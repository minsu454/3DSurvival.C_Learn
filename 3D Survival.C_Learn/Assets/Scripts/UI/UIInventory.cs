using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slotArr;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private PlayerController controller;
    private PlayerCondition condition;

    private ItemData selectedItem;
    private int selectedItemIndex = 0;

    int curEquipIndex;
    // Start is called before the first frame update
    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPos;

        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slotArr = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slotArr.Length; i++)
        {
            slotArr[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slotArr[i].index = i;
            slotArr[i].inventory = this;
        }

        ClearSelectendItemWindow();
    }

    private void ClearSelectendItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    private void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slotArr.Length; i++)
        {
            if (slotArr[i].item != null)
            {
                slotArr[i].Set();
            }
            else
            {
                slotArr[i].Clear();
            }
        }
    }

    private ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slotArr.Length; i++)
        {
            if (slotArr[i].item == data && slotArr[i].quantity < data.maxStackAmount)
            {
                return slotArr[i];
            }
        }
        return null;
    }

    private ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slotArr.Length; i++)
        {
            if (slotArr[i].item == null)
            {
                return slotArr[i];
            }
        }
        return null;
    }

    private void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int index)
    {
        if (slotArr[index].item == null)
            return;

        selectedItem = slotArr[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.consumableArr.Length; i++)
        {
            selectedStatName.text += selectedItem.consumableArr[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.consumableArr[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slotArr[index].equipped);
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slotArr[index].equipped);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if (selectedItem.type != ItemType.Consumable)
            return;

        for (int i = 0; i < selectedItem.consumableArr.Length; i++)
        {
            switch (selectedItem.consumableArr[i].type)
            {
                case ConsumableType.Health:
                    condition.Heal(selectedItem.consumableArr[i].value);
                    break;
                case ConsumableType.Hunger:
                    condition.Eat(selectedItem.consumableArr[i].value);
                    break;
            }
        }

        RemoveSelectedItem();
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    private void RemoveSelectedItem()
    {
        slotArr[selectedItemIndex].quantity--;

        if (slotArr[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slotArr[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectendItemWindow();
        }

        UpdateUI();
    }

    public void OnEquipButton()
    {
        if (slotArr[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }

        slotArr[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equipment.EquipNew(selectedItem);
        UpdateUI();
        SelectItem(selectedItemIndex);
    }

    private void UnEquip(int index)
    {
        slotArr[index].equipped = false;
        CharacterManager.Instance.Player.equipment.UnEquip();

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }
}

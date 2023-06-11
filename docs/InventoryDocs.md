# Inventories

## Models

<details>
<summary>Inventory (Request)</summary>

```csharp
class Inventory
{
    int computerId;
    List<Item> items;
}
```

</details>

<details>
<summary>Inventory (Response)</summary>

```csharp
class Inventory
{
    int computerId;
    List<Item> items;
    long createdOn; //Unix time in seconds
}
```

</details>

<details>
<summary>Item</summary>

```csharp
class Item
{
    string name;
    int size;
    int stackSize;
    int slot; //1-16 (not 0-15)
}
```

</details>

---

## REST Endpoints

<details>
<summary>Add New Inventory</summary>

Add a new inventory

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/inventory/new` |
| Method | `POST` |
| Body | ` Inventory(Request) as JSON `
| Headers | `Authorization` |
| Required Claim | `Service` |
| Success Response | Code: 200|
| Error Response | Code: 400 <br> Content: `JSON is Invalid` |
| Error Response | Code: 401 <br> Content: `Not Authorized` |


</details>

---

<details>
<summary>Get Inventory by ComputerID</summary>

Get the last known inventory, it also sends a request to the turtle for an inventory update so listen to the websocket for an updated inventory.

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/inventory/get/by-id` |
| Method | `GET` |
| URL Params | `computerId: int` |
| Headers | `Authorization` |
| Required Claim | `Guest` |
| Success Response | Code: 200 <br> Content: `Inventory(Response) as JSON` |
| Error Response | Code: 401 <br> Content: `Not Authorized` |
| Error Response | Code: 404 <br> Content: `Inventory of Computer by ID: [id] not found` |

</details>

---

## Websocket (SignalR)
Clients should use a SignalR library/SDK.

<details>
<summary>Add New Inventory</summary>

Add a new inventory over websocket

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/ws/server` |
| Headers | `Authorization` |
| Required Claim | `Service` |
| Target | `NewInventory` |
| Arguments | `Inventory(Request) as JSON` |
| Success Response | Code: 200 <br> Content: `Ok` |
| Error Response | Code: 400 <br> Content: `Invalid Model` |
| Error Response | Code: 401 <br> Content: `Not Authorized` |


</details>

---

<details>
<summary>Listen for new Location</summary>

Receive a new location when it has been sent

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/ws/client` |
| Headers | `Authorization` |
| Required Claim | `Guest` |
| Method | `NewInventory` |
| Success Response | Code: 200 <br> Method: `NewInventory` <br> Content: `Inventory(Response) as JSON` |

</details>


---

<details>
<summary>Listen for new inventory request</summary>

Receive new inventory requests

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/ws/server` |
| Headers | `Authorization` |
| Required Claim | `Service` |
| Method | `RequestInventory` |
| Success Response | Code: 200 <br> Method: `RequestInventory` <br> Content: `ComputerID as Int` |

</details>

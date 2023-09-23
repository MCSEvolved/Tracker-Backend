# Computers/Turtles
Computer is a collective name for both computers and turtles

## Models

<details>
<summary>Computer (Request)</summary>

```csharp
class Computer
{
    int id;
    string? label;
    int systemId;
    ComputerDevice device;
    int? fuelLimit;
    string? status;
    int? fuelLevel;
    bool hasModem;
}
```

</details>

<details>
<summary>Computer (Response)</summary>

```csharp
class Computer
{
    int id;
    string? label;
    int systemId;
    ComputerDevice device;
    int? fuelLimit;
    string? status;
    int? fuelLevel;
    long lastUpdate; //Unix time in seconds
    bool hasModem;
}
```

</details>

---
## Enums

<details>
<summary>ComputerDevice</summary>

```csharp
enum ComputerDevice
{
    INVALID, //Only used to check if ComputerDevice is not null
    Turtle,
    Advanced_Turtle,
    Computer,
    Advanced_Computer,
    Pocket_Computer,
    Advanced_Pocket_Computer
}
```

</details>

---
## REST Endpoints

<details>
<summary>Add New Computer</summary>

Add/update a computer, only the Minecraft server should do this.

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/computer/new` |
| Method | `POST` |
| Body | ` Computer(Request) as JSON `
| Headers | `Authorization` |
| Required Claim | `Service` |
| Success Response | Code: 200|
| Error Response | Code: 400 <br> Content: `JSON is Invalid` |
| Error Response | Code: 401 <br> Content: `Not Authorized` |


</details>

---

<details>
<summary>Get All Computers</summary>

Get a list of all the computers in the database

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/computer/get/all` |
| Method | `GET` |
| Headers | `Authorization` |
| Required Claim | `Guest` |
| Success Response | Code: 200 <br> Content: `List<Computer(Response)> as JSON` |
| Error Response | Code: 401 <br> Content: `Not Authorized` |
| Error Response | Code: 404 <br> Content: `No Computers Found` |

</details>

---

<details>
<summary>Get Computer by ID</summary>

Get a computer by its ComputerId

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/computer/get/by-id` |
| Method | `GET` |
| URL Params | `id: int` |
| Headers | `Authorization` |
| Required Claim | `Guest` |
| Success Response | Code: 200 <br> Content: `Computer(Response) as JSON` |
| Error Response | Code: 401 <br> Content: `Not Authorized` |
| Error Response | Code: 404 <br> Content: `Computer by ID: [id] not found` |

</details>

---

<details>
<summary>Get Computers(Ids) by System</summary>

Get a computer by its corresponding system, you can also only get the IDs by passing idsOnly = true as a parameter

| Name | Value                                                                |
| --- |----------------------------------------------------------------------|
| URL | `api.mcsynergy.nl/tracker/computer/get/by-system`                    |
| Method | `GET`                                                                |
| URL Params | `systemId: int` <br> `idsOnly: bool`                                     |
| Headers | `Authorization`                                                      |
| Required Claim | `Guest`                                                              |
| Success Response | Code: 200 <br> Content: `List<Computer(Response)> as JSON`           |
| Error Response | Code: 401 <br> Content: `Not Authorized`                             |
| Error Response | Code: 404 <br> Content: `No computers found with system: [systemId]` |

</details>

## Websocket (SignalR)
Clients should use a SignalR library/SDK.

<details>
<summary>Add New Computer</summary>

Add a new computer over websocket

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/ws/server` |
| Headers | `Authorization` |
| Required Claim | `Service` |
| Target | `NewComputer` |
| Arguments | `Computer(Request) as JSON` |
| Success Response | Code: 200 <br> Content: `Ok` |
| Error Response | Code: 400 <br> Content: `Invalid Model` |
| Error Response | Code: 401 <br> Content: `Not Authorized` |


</details>

---

<details>
<summary>Listen for Computer Updates</summary>

Receive a new computer when it has been updated

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/ws/client` |
| Headers | `Authorization` |
| Required Claim | `Guest` |
| Method | `NewComputer` |
| Success Response | Code: 200 <br> Method: `NewComputer` <br> Content: `Computer(Response) as JSON` |

</details>
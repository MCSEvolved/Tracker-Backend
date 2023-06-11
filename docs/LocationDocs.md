# Location

## Models

<details>
<summary>Location (Request)</summary>

```csharp
class Location
{
    int computerId;
    Coordinates coordinates;
    string dimension;
}
```

</details>

<details>
<summary>Location (Response)</summary>

```csharp
class Location
{
    int computerId;
    Coordinates coordinates;
    string dimension;
    long createdOn; //Unix time in seconds
}
```

</details>

<details>
<summary>Coordinates</summary>

```csharp
class Coordinates
{
    int x;
    int y;
    int z;
}
```

</details>

---

## REST Endpoints

<details>
<summary>Get Location by ComputerID</summary>

Get the location of a turtle by its ComputerId

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/location/get/by-id` |
| Method | `GET` |
| URL Params | `computerId: int` |
| Headers | `Authorization` |
| Required Claim | `Guest` |
| Success Response | Code: 200 <br> Content: `Location(Response) as JSON` |
| Error Response | Code: 401 <br> Content: `Not Authorized` |
| Error Response | Code: 404 <br> Content: `Location of Computer by ID: [id] not found` |

</details>

---

## Websocket (SignalR)
Clients should use a SignalR library/SDK.

<details>
<summary>Add New Location</summary>

Add a new location over websocket

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/ws/server` |
| Headers | `Authorization` |
| Required Claim | `Service` |
| Target | `NewLocation` |
| Arguments | `Location(Request) as JSON` |
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
| Method | `NewLocation` |
| Success Response | Code: 200 <br> Method: `NewLocation` <br> Content: `Location(Response) as JSON` |

</details>

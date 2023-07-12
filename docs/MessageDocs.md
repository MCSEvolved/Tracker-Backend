# Messages

## Models

<details>
<summary>Message (Request)</summary>

```csharp
class Message
{
    MessageType type;
    MessageSource source;
    string content;
    JSON? metaData;
    string sourceId; //ComputerID, SystemID or Service Name
}
```

</details>

<details>
<summary>Message (Response)</summary>

```csharp
class Message
{
    string id;
    MessageType type;
    MessageSource source;
    string content;
    JSON? metaData;
    string sourceId; //ComputerID, SystemID or Service Name
    long creationTime; //Unix time in milliseconds
}
```

</details>

---
## Enums

<details>
<summary>MessageType</summary>

```csharp
enum MessageType
	{
        INVALID, //Only used to check if MessageType is not null
        Error,
        Warning,
        Info,
        Debug,
        OutOfFuel
    }
```

</details>

<details>
<summary>MessageSource</summary>

```csharp
enum MessageSource
	{
        INVALID, //Only used to check if MessageSource is not null
        Service,
        Computer,
        Turtle,
        System
    }
```

</details>

---

## REST Endpoints

<details>
<summary>Add New Message</summary>

Add a new message, other services can also use this to sent logs of their service.

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/message/new` |
| Method | `POST` |
| Body | ` Message(Request) as JSON `
| Headers | `Authorization` |
| Required Claim | `Service` |
| Success Response | Code: 200|
| Error Response | Code: 400 <br> Content: `JSON is Invalid` |
| Error Response | Code: 401 <br> Content: `Not Authorized` |


</details>

---

<details>
<summary>Get Messages (pagination + filters)</summary>

Get a list of all the messages

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/message/get` |
| Method | `GET` |
| URL Params | `page: int` <br> `pageSize: int ` <br> `MessageFilter (see example!)` |
| Headers | `Authorization` |
| Required Claim | `Guest` |
| Success Response | Code: 200 <br> Content: `List<Message(Response)> as JSON` |
| Error Response | Code: 400 <br> Content: `Bad Request` |
| Error Response | Code: 401 <br> Content: `Not Authorized` |
| Error Response | Code: 403 <br> Content: `Forbidden` |

Example Request:
```csharp
//These are all the possible filters you can add
class MessageFilter
	{
		MessageType[]? types;
		MessageSource[]? sources;
		long? beginRange;
		long? endRange;
		string[]? sourceIds
	}
```

This is how you use them:  
`.../message/get?page=1&pageSize=20&types=Error&types=Warning&sourceIds=4&sourceIds=8`

Here you will get the first 20 messages with the types `Error` and `Warning` and sourceIds `4` and `8`.
You can add as many of these filters as you want.


</details>

---

<details>
<summary>Get Amount Messages (filters)</summary>

Get the amount of messages

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/message/get/amount` |
| Method | `GET` |
| URL Params | `MessageFilter (see example!)` |
| Headers | `Authorization` |
| Required Claim | `Guest` |
| Success Response | Code: 200 <br> Content: `Amount as float` |
| Error Response | Code: 400 <br> Content: `Bad Request` |
| Error Response | Code: 401 <br> Content: `Not Authorized` |
| Error Response | Code: 403 <br> Content: `Forbidden` |

Example Request:
```csharp
//These are all the possible filters you can add
class MessageFilter
	{
		MessageType[]? types;
		MessageSource[]? sources;
		long? beginRange;
		long? endRange;
		string[]? sourceIds
	}
```

This is how you use them:  
`.../message/get/amount?types=Error&types=Warning&sourceIds=4&sourceIds=8`

Here you will get the amount of messages with the types `Error` and `Warning` and sourceIds `4` and `8`.
You can add as many of these filters as you want.


</details>

---

<details>
<summary>[DEPRECATED] Get All Messages (pagination)</summary>

Get a list of all the messages

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/message/get/all` |
| Method | `GET` |
| URL Params | `page: int` <br> `pageSize: int ` |
| Headers | `Authorization` |
| Required Claim | `Guest` |
| Success Response | Code: 200 <br> Content: `List<Message(Response)> as JSON` |
| Error Response | Code: 401 <br> Content: `Not Authorized` |
| Error Response | Code: 404 <br> Content: `No Messages Found` |


</details>

---

<details>
<summary>[DEPRECATED] Get All Messages by Source (pagination)</summary>

Get a list of all the messages by source

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/message/get/by-source` |
| Method | `GET` |
| URL Params | `page: int` <br> `pageSize: int ` <br> `source: MessageSource` |
| Headers | `Authorization` |
| Required Claim | `Guest` |
| Success Response | Code: 200 <br> Content: `List<Message(Response)> as JSON` |
| Error Response | Code: 401 <br> Content: `Not Authorized` |
| Error Response | Code: 404 <br> Content: `No Messages Found` |


</details>

---

<details>
<summary>[DEPRECATED] Get All Messages by Source IDs (pagination)</summary>

Get a list of all the messages by source IDs

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/message/get/by-source-ids` |
| Method | `GET` |
| URL Params | `page: int` <br> `pageSize: int ` <br> `sourceIds: array[string]` |
| Headers | `Authorization` |
| Required Claim | `Guest` |
| Success Response | Code: 200 <br> Content: `List<Message(Response)> as JSON` |
| Error Response | Code: 401 <br> Content: `Not Authorized` |
| Error Response | Code: 404 <br> Content: `No Messages Found` |


</details>

---

## Websocket (SignalR)
Clients should use a SignalR library/SDK.

<details>
<summary>Add New Message</summary>

Add a new message over websocket

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/ws/server` |
| Headers | `Authorization` |
| Required Claim | `Service` |
| Target | `NewMessage` |
| Arguments | `Message(Request) as JSON` |
| Success Response | Code: 200 <br> Content: `Ok` |
| Error Response | Code: 400 <br> Content: `Invalid Model` |
| Error Response | Code: 401 <br> Content: `Not Authorized` |


</details>

---

<details>
<summary>Listen for new Messages</summary>

Receive a new message when it has been sent

| Name | Value |
| --- | --- |
| URL | `api.mcsynergy.nl/tracker/ws/client` |
| Headers | `Authorization` |
| Required Claim | `Guest` |
| Method | `NewMessage` |
| Success Response | Code: 200 <br> Method: `NewMessage` <br> Content: `Message(Response) as JSON` |

</details>
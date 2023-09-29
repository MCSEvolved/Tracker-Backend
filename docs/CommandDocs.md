# Commands
Send commands to turtles and computers

Current commands are:

- STOP - Stops the main script
- REBOOT - Reboots the computer/turtle, make sure that the script runs on startup otherwise youre unable to access it!

---
## REST Endpoints

<details>
<summary>Send command to computer</summary>

Send a command to a computer

| Name | Value                                        |
| --- |----------------------------------------------|
| URL | `api.mcsynergy.nl/tracker/command/execute`   |
| Method | `POST`                                       |
| Headers | `Authorization`                              |
| Required Claim | `Player`                                     |
| URL Params | `computerIds: int[]` <br> `command: string ` |
| Success Response | Code: 200                                    |
| Error Response | Code: 400 <br> Content: `Missing params`     |
| Error Response | Code: 400 <br> Content: `Invalid Command`    |
| Error Response | Code: 401 <br> Content: `Not Authorized`     |
| Error Response | Code: 403 <br> Content: `Forbidden`          |


</details>

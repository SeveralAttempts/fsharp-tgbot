namespace Laminary.Enums

module CommandsEnum =
    type AvailableCommands = 
        | HelpCommand
        | LoginCommand
        | RegisterCommand
        | CheckLoginInfo
        | UnknownCommand

    // let parseAvailableCommands =
    //     | "/help" -> HelpCommand
    //     | "/login" -> LoginCommand
    //     | "/register" -> RegisterCommand
    //     | "checklogin" -> CheckLoginInfo
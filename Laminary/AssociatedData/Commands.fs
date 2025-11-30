namespace Laminary.AssociatedData

module Commands =
    type AvailableCommands = 
        | HelpCommand
        | LoginCommand
        | RegisterCommand
        | CheckLoginInfo
        | UnknownCommand

    let ParseAvailableCommands message =
        match message with
            | "/help" -> HelpCommand
            | "/login" -> LoginCommand
            | "/register" -> RegisterCommand
            | "/checkLogin" -> CheckLoginInfo
            | _ -> UnknownCommand

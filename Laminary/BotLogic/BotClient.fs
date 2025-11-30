module BotClient

open System
open Telegram.Bot
open Telegram.Bot.Types
open Telegram.Bot.Types.Enums
open Laminary.AssociatedData.Commands
open Laminary.AssociatedData.States

let state = State()

let HelpCommandExecute() = 
    "This bot generate uuids.\nCommands:\n
    1./help - get this message with bot and commands descriptions.\n
    2./register - register new account.\n
    3./myinfo - check if you have current login session.\n
    4./login - login in new account"

let HandleUnknownCommand() =
    "No such command provided by the bot."

let RegisterCommandExecute() =
    if state.GetRegisterState = RegisterState.NotProcessing then
        state.SetRegisterState RegisterState.SetName
        "Please, enter your name to register:"
    elif state.GetRegisterState = RegisterState.SetName then
        state.SetRegisterState RegisterState.SetPassword
        "Name received. Please, enter your password to register:"
    elif state.GetRegisterState = RegisterState.SetPassword then
        state.SetRegisterState RegisterState.EndRegister
        "Registration completed successfully!"
    else
        state.SetRegisterState RegisterState.NotProcessing
        "Registration process cancelled."

let LoginCommandExecute() =
    if state.GetLoginState = LoginState.NotProcessing then
        state.SetLoginState LoginState.EnterName
        "Please, enter your name to login:"
    elif state.GetLoginState = LoginState.EnterName then 
        state.SetLoginState LoginState.EnterPassword
        "Name received. Please, enter password to login:"
    elif state.GetLoginState = LoginState.EnterPassword then
        state.SetLoginState LoginState.EndLogin
        "Login completed successfully!"
    else
        state.SetLoginState LoginState.NotProcessing
        "Login process cancelled."
    
let CheckLoginInfoCommandExecute() =
    // TODO: login info from DB -> need async
    "No login info."

let ParseCommandsToAction command =
    match command with
        | HelpCommand -> HelpCommandExecute()
        | RegisterCommand -> RegisterCommandExecute()
        | LoginCommand -> LoginCommandExecute()
        | CheckLoginInfo -> CheckLoginInfoCommandExecute()
        | UnknownCommand -> HandleUnknownCommand()

let InitializeBot(token: string) =
    if String.IsNullOrEmpty token then
        failwith "Bot token is null or empty!"
    let botClient = TelegramBotClient token
    botClient

let OnMsgReceived (client: ITelegramBotClient) (token: Threading.CancellationToken) (message: Message) (updateType: UpdateType) =
    async {
        if message.Text = null then return ()

        let command = ParseAvailableCommands(message.Text.Trim().ToLower())        
        let messageToSend = ParseCommandsToAction command

        if state.IsLoginProcessing && message.Text.StartsWith('/') <> true then
            let loginMessage = LoginCommandExecute()
            let! _ = client.SendMessage(message.Chat, loginMessage, cancellationToken = token) |> Async.AwaitTask
            ()
        elif state.IsRegisterProcessing && message.Text.StartsWith('/') <> true then
            let registerMessage = RegisterCommandExecute()
            let! _ = client.SendMessage(message.Chat, registerMessage, cancellationToken = token) |> Async.AwaitTask
            ()
        else
            let! _ = client.SendMessage(message.Chat, messageToSend, cancellationToken = token) |> Async.AwaitTask
            ()
    } |> Async.StartAsTask :> Threading.Tasks.Task

let StartBot: Async<unit> = 
    async {
        do! Async.Sleep Threading.Timeout.Infinite
        return ()
    }
module BotClient

open System
open Telegram.Bot
open Telegram.Bot.Types
open Telegram.Bot.Types.Enums
open Laminary.AssociatedData.Commands
open Laminary.AssociatedData.States

let state = State()

let HelpCommandExecute = 
    "This bot generate uuids.\nCommands:\n
    1./help - get this message with bot and commands descriptions.\n
    2./register - register new account.\n
    3./myinfo - check if you have current login session.\n
    4./login - login in new account"

let HandleUnknownCommand =
    "No such command provided by the bot."

let RegisterCommandExecute =
    if state.GetRegisterState = RegisterState.NotProcessing then
        state.SetRegisterState SetName
        "Please, enter your name to register:"
    else if state.GetRegisterState = SetName then
        state.SetRegisterState SetPassword
        "Name received. Please, enter your password to register:"
    else if state.GetRegisterState = SetPassword then
        state.SetRegisterState EndRegister
        "Registration completed successfully!"
    else
        state.SetRegisterState RegisterState.NotProcessing
        "Registration process cancelled."

let InitializeBot(token: string) =
    if String.IsNullOrEmpty token then
        failwith "Bot token is null or empty!"
    let botClient = TelegramBotClient token
    botClient

let OnMsgReceived (client: ITelegramBotClient) (token: Threading.CancellationToken) (message: Message) (updateType: UpdateType) =
    async {
        if message.Text = null then return ()

        let command = parseAvailableCommands(message.Text.Trim().ToLower())

        let messageToSend =
            match command with
                | HelpCommand -> HelpCommandExecute
                | RegisterCommand -> RegisterCommandExecute
                | UnknownCommand -> HandleUnknownCommand

        let! _ = client.SendMessage(message.Chat, messageToSend, cancellationToken = token) |> Async.AwaitTask
        ()
    } |> Async.StartAsTask :> Threading.Tasks.Task

let StartBot: Async<unit> = 
    async {
        do! Async.Sleep Threading.Timeout.Infinite
        return ()
    }
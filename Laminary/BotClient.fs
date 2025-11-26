module BotClient

open System
open Telegram.Bot
open Telegram.Bot.Types
open Telegram.Bot.Types.Enums

type AvailableCommands = 
    | HelpCommand
    | GenerateUuidCommand
    | UnknownCommand

let HelpCommandExecute = 
    let message = "This bot generate uuids.\nCommands:\n
    1./help - get this message with bot and commands descriptions.\n
    2./generate - generate new uuid.\n"
    message

let GenerateUuidCommandExecute() =
    Guid.NewGuid().ToString()

let HandleUnknownCommand =
    let message = "No such command provided by the bot."
    message

let InitializeBot(token: string) =
    if String.IsNullOrEmpty token then
        failwith "Bot token is null or empty!"
    let botClient = TelegramBotClient(token)
    botClient

let OnMsgReceived (client: ITelegramBotClient) (token: Threading.CancellationToken) (message: Message) (updateType: UpdateType) =
    async {
        if message.Text = null then return ()

        let command = 
            match message.Text.Trim().ToLower() with 
                | "/help" -> HelpCommand
                | "/generate" -> GenerateUuidCommand
                | _ -> UnknownCommand

        let messageToSend =
            match command with
                | HelpCommand -> HelpCommandExecute
                | GenerateUuidCommand -> GenerateUuidCommandExecute()
                | UnknownCommand -> HandleUnknownCommand

        let! _ = client.SendMessage(message.Chat, messageToSend, cancellationToken = token) |> Async.AwaitTask
        ()
    } |> Async.StartAsTask :> Threading.Tasks.Task

let StartBot: Async<unit> = 
    async {
        do! Async.Sleep Threading.Timeout.Infinite
        return ()
    }
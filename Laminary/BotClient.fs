module BotClient

open System
open Telegram.Bot
open Telegram.Bot.Types
open Telegram.Bot.Types.Enums

let InitializeBot(token: string) =
    if String.IsNullOrEmpty token then
        failwith "Bot token is null or empty!"
    let botClient = TelegramBotClient(token)
    botClient

let OnMsgReceived (client: ITelegramBotClient) (token: Threading.CancellationToken) (message: Message) (updateType: UpdateType) =
    async {
        if message.Text = null then return ()
        let! _ = Async.Sleep(1000) |> Async.StartChild
        let! _ = client.SendMessage(message.Chat, $"You said: {message.Text}", cancellationToken = token) |> Async.AwaitTask
        ()
    } |> Async.StartAsTask :> Threading.Tasks.Task

let StartBot: Async<unit> = 
    async {
        do! Async.Sleep Threading.Timeout.Infinite
        return ()
    }
open System
open Telegram.Bot
open Telegram.Bot.Types
open Telegram.Bot.Types.Enums
open Microsoft.Extensions.Configuration



let mutable cancellationTokenSource = new Threading.CancellationTokenSource()

let InitializeBot(token: string) =
    if String.IsNullOrEmpty token then
        failwith "Bot token is null or empty!"
    let botClient = TelegramBotClient(token)
    botClient

let config =
        ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.Development.json", optional = true, reloadOnChange = true)
            .AddJsonFile("appsettings.json", optional = true, reloadOnChange = true)
            .Build()
let token = config.GetValue<string> "Telegram:BotToken" 
let client = InitializeBot token

let OnMsgReceived (message: Message) (updateType: UpdateType) =
    async {
        if message.Text = null then return ()
        cancellationTokenSource.Cancel()
        cancellationTokenSource.Dispose()
        cancellationTokenSource <- new Threading.CancellationTokenSource()
        let token = cancellationTokenSource.Token
        let! _ = Async.Sleep(1000) |> Async.StartChild
        let! _ = client.SendMessage(message.Chat, $"You said: {message.Text}", cancellationToken = token ) |> Async.AwaitTask
        ()
    } |> Async.StartAsTask :> Threading.Tasks.Task

let handler = TelegramBotClient.OnMessageHandler OnMsgReceived
client.add_OnMessage handler

let StartBot: Async<unit> = 
    async {
        do! Async.Sleep Threading.Timeout.Infinite
        return ()
    }

StartBot |> Async.RunSynchronously
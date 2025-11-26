open System
open Microsoft.Extensions.Configuration
open Telegram.Bot
open BotClient

let mutable cancellationTokenSource = new Threading.CancellationTokenSource()

let config =
        ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.Development.json", optional = true, reloadOnChange = true)
            .AddJsonFile("appsettings.json", optional = true, reloadOnChange = true)
            .Build()

let token = config.GetValue<string> "Telegram:BotToken" 
let client = InitializeBot token
let handler = TelegramBotClient.OnMessageHandler (fun msg upd -> OnMsgReceived client cancellationTokenSource.Token msg upd)
client.add_OnMessage handler

StartBot |> Async.RunSynchronously
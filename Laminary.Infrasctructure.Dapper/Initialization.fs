namespace Laminary.Infrasctructure.Dapper

open Dapper.FSharp.PostgreSQL
open DbUp

module Base = 
    let RunMigrations connectionString =
        DeployChanges.To
            .PostgresqlDatabase(connectionString).

    OptionTypes.register()
    // let connection: IDbConnection = 
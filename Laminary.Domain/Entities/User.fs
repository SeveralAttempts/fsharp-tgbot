namespace Laminary.Domain.Entities

open System
open Laminary.Domain.Helpers.Error

module UserEntity =
    type private User =
        { 
            Id: Guid
            Name: string 
        }

    // let CreateUser(id: Guid, name: string): Result<User, string> =
    //     Ok { Id = id; Name = name }
namespace Laminary.Domain.Entities

open System
open Laminary.Domain.Helpers.DomainError

module UserEntity =
    type User = 
        private { 
            Id: Guid
            Name: string
            RegistrationDate: DateTime
            Password: string
        }

    let CreateUser(id: Guid, name: string, registrationDate: DateTime, password: string): Result<User, DomainError> =
        let isIdEmpty = id = Guid.Empty
        let isNameLengthCorrect = name.Length > 40 || name.Length < 1
        let isPasswordLengthCorrect = password.Length < 6

        if isIdEmpty || isNameLengthCorrect || isPasswordLengthCorrect then
            Error Validation
        else 
            Ok { Id = id; Name = name; RegistrationDate = registrationDate; Password = password }
namespace Laminary.AssociatedData

module States = 
    type RegisterState =
        | SetName
        | SetPassword
        | EndRegister

    type LoginState =
        | EnterName
        | EnterPassword
        | CheckIfExists
        | EndLogin

    type GeneralState =
        | Idle
        | Register of RegisterState
        | Login  of LoginState

    type State() =
        static member Current: GeneralState = Idle
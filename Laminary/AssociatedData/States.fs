namespace Laminary.AssociatedData

module States = 
    type RegisterState =
        | SetName
        | SetPassword
        | EndRegister
        | NotProcessing

    type LoginState =
        | EnterName
        | EnterPassword
        | CheckIfExists
        | EndLogin
        | NotProcessing

    type GeneralState =
        | Idle
        | Register
        | Login

    type State() =
        let mutable generalState: GeneralState = Idle
        let mutable innerRegisterState: RegisterState = RegisterState.NotProcessing
        let mutable innerLoginState: LoginState = LoginState.NotProcessing

        member public this.SetIdleState() =
            generalState <- Idle
            innerRegisterState <- RegisterState.NotProcessing
            innerLoginState <- LoginState.NotProcessing
        
        member public this.SetRegisterState(state: RegisterState) =
            if generalState.IsRegister <> true then
                generalState <- Register
            innerRegisterState <- state

        member public this.SetLoginState(state: LoginState) =
            if generalState.IsLogin <> true then
                generalState <- Login
            innerLoginState <- state

        member public this.GetGeneralState = generalState
        member public this.GetRegisterState = innerRegisterState
        member public this.GetLoginState = innerLoginState

namespace Laminary.AssociatedData

module States = 
    type RegisterState =
        | SetName
        | SetPassword
        | EndRegister
        | NotProcessing
        | Cancel

    type LoginState =
        | EnterName
        | EnterPassword
        | CheckIfExists
        | EndLogin
        | NotProcessing
        | Cancel

    type GeneralState =
        | Idle
        | Register
        | Login

    type State(isRegisterProcessing, isLoginProcessing,
        registerNotProcessing, loginNotProcessing, initialState) =
        let mutable generalState: GeneralState = initialState
        let mutable innerRegisterState: RegisterState = registerNotProcessing
        let mutable innerLoginState: LoginState = loginNotProcessing
        let mutable isRegisterProcessing: bool = isRegisterProcessing
        let mutable isLoginProcessing: bool = isLoginProcessing

        member public this.SetIdleState() =
            generalState <- Idle
            innerRegisterState <- RegisterState.NotProcessing
            innerLoginState <- LoginState.NotProcessing
            isLoginProcessing <- false
            isRegisterProcessing <- false
        
        member public this.SetRegisterState(state: RegisterState) =
            if isRegisterProcessing <> true then
                generalState <- Register
                isRegisterProcessing <- true
                isLoginProcessing <- false
            innerRegisterState <- state
            if innerRegisterState = RegisterState.EndRegister then
                isRegisterProcessing <- false
                generalState <- Idle

        member public this.SetLoginState(state: LoginState) =
            if isLoginProcessing <> true then
                generalState <- Login
                isRegisterProcessing <- false
                isLoginProcessing <- true
            innerLoginState <- state
            if innerLoginState = LoginState.EndLogin then
                isLoginProcessing <- false
                generalState <- Idle
        
        new() = State(false, false, RegisterState.NotProcessing, 
            LoginState.NotProcessing, Idle)

        member public this.GetGeneralState = generalState
        member public this.GetRegisterState = innerRegisterState
        member public this.GetLoginState = innerLoginState

        member public this.IsRegisterProcessing = isRegisterProcessing
        member public this.IsLoginProcessing = isLoginProcessing


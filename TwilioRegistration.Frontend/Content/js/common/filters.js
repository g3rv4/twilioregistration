(function () {
    app.filter("errorMessage", function () {
        return function (value) {
            switch (value) {
                case 'TwilioRegistration.DataTypes.Exceptions.InvalidUsernameOrPasswordException': return 'Invalid email or password'
                case 'TwilioRegistration.DataTypes.Exceptions.AccountInactiveException': return 'Your account is inactive'
                case 'TwilioRegistration.DataTypes.Exceptions.AccountTemporarilyDisabledException': return 'Your account has been temporarily disabled due to many unsuccessful login attempts. Try again later.'
                case 'TwilioRegistration.DataTypes.Exceptions.UsernameTakenException': return 'The username you chose is already used by a different device'
                default: return 'Unknown error: ' + value
            }
        }
    })
})();
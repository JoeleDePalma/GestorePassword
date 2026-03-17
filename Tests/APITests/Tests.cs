using System;
using System.Collections.Generic;
using System.Text;
using Libreria.API;
using NUnit.Framework;
using Libreria.DTOs.Users;
using Tests.APITests.Users.Services;
using Tests.APITests.Passwords.Services;
using Libreria.DTOs.Passwords;

namespace Tests.APITests
{
    public class Tests : ApiTestsConfiguration
    {
        private UserApiTestService _userService;
        private PasswordApiTestService _passwordService;

        private int _firstUserId;
        private int _secondUserId;

        private int _firstUserFirstPasswordId;
        private int _firstUserSecondPasswordId;

        private string _firstToken;
        private string _secondToken;


        [SetUp]
        public void Init()
        {
            _userService = new UserApiTestService(Client);
            _passwordService = new PasswordApiTestService(Client);
        }

        [Category("UserAPI")]
        [Order(1)]
        [Test]
        public async Task UserCreate_ShouldReturnDTO1()
            => await Execute(() => _userService.RegisterAsync("UsernameTest1", "PasswordUser1"));

        [Category("UserAPI")]
        [Order(2)]
        [Test]
        public async Task UserCreate_ShouldReturnDTO2()
            => await Execute(() => _userService.RegisterAsync("UsernameTest2", "PasswordUser2"));

        [Category("UserAPI")]
        [Order(3)]
        [Test]
        public async Task UserLogin_ShouldReturnToken1()
        {
            _firstToken = await _userService.LoginAsync("UsernameTest1", "PasswordUser1");
        }

        [Category("UserAPI")]
        [Order(4)]
        [Test]
        public async Task UserLogin_ShouldReturnToken2()
        {
            _secondToken = await _userService.LoginAsync("UsernameTest2", "PasswordUser2");
        }

        [Category("UserAPI")]
        [Order(5)]
        [Test]
        public async Task UserUpdate_ShouldReturnTrue1()
            => await Execute(() => _userService.UpdateAsync("UsernameTest1Updated", "NewPasswordUser1", _firstToken));

        [Category("UserAPI")]
        [Order(6)]
        [Test]
        public async Task UserUpdate_ShouldReturnTrue2()
            => await Execute(() => _userService.UpdateAsync("UsernameTest2Updated", "NewPasswordUser2", _secondToken));

        [Category("UserAPI")]
        [Order(7)]
        [Test]
        public async Task UserGetByToken_ShouldReturnDTO1()
        {
            _firstUserId = await _userService.GetByTokenAsync(_firstToken, "UsernameTest1Updated");
        }

        [Category("UserAPI")]
        [Order(8)]
        [Test]
        public async Task UserGetByToken_ShouldReturnDTO2()
        {
            _secondUserId = await _userService.GetByTokenAsync(_secondToken, "UsernameTest2Updated");
        }

        [Category("UserAPI")]
        [Order(9)]
        [Test]
        public async Task UserGetById_ShouldReturnDTO1()
            => await Execute(() => _userService.GetByIdAsync(_firstUserId, "UsernameTest1Updated"));

        [Category("UserAPI")]
        [Order(10)]
        [Test]
        public async Task UserGetById_ShouldReturnDTO2()
            => await Execute(() => _userService.GetByIdAsync(_secondUserId, "UsernameTest2Updated"));

        [Category("PasswordAPI")]
        [Order(11)]
        [Test]
        public async Task FirstUserPasswordCreate_ShouldReturnDTO1()
        {
            _firstUserFirstPasswordId = await _passwordService.CreateAsync
                (
                    token: _firstToken,
                    NewPassword: new CreatePasswordDTO()
                    {
                        AppName = "Instagram",
                        AppUsername = "UserInstagram",
                        Password = "PasswordInstagram",
                        MasterPassword = "NewPasswordUser1"
                    }
                );
        }

        [Category("PasswordAPI")]
        [Order(12)]
        [Test]
        public async Task FirstUserPasswordCreate_ShouldReturnDTO2()
        {
            _firstUserSecondPasswordId = await _passwordService.CreateAsync
                (
                    token: _firstToken,
                    NewPassword: new CreatePasswordDTO()
                    {
                        AppName = "TikTok",
                        AppUsername = "UserTikTok",
                        Password = "PasswordTikTok",
                        MasterPassword = "NewPasswordUser1"
                    }
                );
        }

        [Category("PasswordAPI")]
        [Order(13)]
        [Test]
        public async Task FirstUserPasswordUpdateByApp_ShouldReturnDTO1()
            => await Execute(() => _passwordService.UpdateByAppAsync
            (
                token: _firstToken,
                UpdatedPassword: new UpdatePasswordDTO()
                {
                    AppName = "NewInstagram",
                    AppUsername = "NewUserInstagram",
                    Password = "NewPasswordInstagram",
                    MasterPassword = "NewPasswordUser1"
                },
                passwordApp: "Instagram"
            ));

        [Category("PasswordAPI")]
        [Order(14)]
        [Test]
        public async Task FirstUserPasswordUpdateById_ShouldReturnDTO1()
            => await Execute(() => _passwordService.UpdateByIdAsync
            (
                token: _firstToken,
                UpdatedPassword: new UpdatePasswordDTO()
                {
                    AppName = "NewTikTok",
                    AppUsername = "NewUserTikTok",
                    Password = "NewPasswordTikTok",
                    MasterPassword = "NewPasswordUser1"
                },
                passwordId: _firstUserSecondPasswordId
            ));

        [Category("PasswordAPI")]
        [Order(15)]
        [Test]
        public async Task FirstUserPasswordGetAll_ShouldReturnDTOsList1()
            => await Execute(() => _passwordService.GetAllAsync
            (
                token: _firstToken,
                masterPassword: "NewPasswordUser1",
                appExpected: "NewInstagram",
                usernameExpected: "NewUserInstagram",
                passwordExpected: "NewPasswordInstagram"
            ));

        [Category("PasswordAPI")]
        [Order(16)]
        [Test]
        public async Task FirstUserPasswordGetById_ShouldReturnDTO1()
            => await Execute(() => _passwordService.GetByIdAsync
            (
                token: _firstToken,
                passwordId: _firstUserFirstPasswordId,
                masterPassword: "NewPasswordUser1",
                appExpected: "NewInstagram",
                usernameExpected: "NewUserInstagram",
                passwordExpected: "NewPasswordInstagram"
            ));

        [Category("PasswordAPI")]
        [Order(17)]
        [Test]
        public async Task FirstUserPasswordGetByApp_ShouldReturnDTO1()
            => await Execute(() => _passwordService.GetByAppAsync
            (
                token: _firstToken,
                passwordApp: "NewTikTok",
                masterPassword: "NewPasswordUser1",
                appExpected: "NewTikTok",
                usernameExpected: "NewUserTikTok",
                passwordExpected: "NewPasswordTikTok"
            ));

        [Category("PasswordAPI")]
        [Order(18)]
        [Test]
        public async Task FirstUserPasswordDeleteById_ShouldReturnTrue1()
            => await Execute(() => _passwordService.DeleteByIdAsync
            (
                token: _firstToken,
                passwordId: _firstUserFirstPasswordId
            ));

        [Category("PasswordAPI")]
        [Order(19)]
        [Test]
        public async Task FirstUserPasswordDeleteByApp_ShouldReturnTrue1()
            => await Execute(() => _passwordService.DeleteByAppAsync
            (
                token: _firstToken,
                passwordApp: "NewTikTok"
            ));

        [Category("UserAPI")]
        [Order(20)]
        [Test]
        public async Task UserDeleteByToken_ShouldReturnTrue1()
            => await Execute(() => _userService.DeleteByTokenAsync(_firstToken));

        [Category("UserAPI")]
        [Order(21)]
        [Test]
        public async Task UserDeleteByToken_ShouldReturnTrue2()
            => await Execute(() => _userService.DeleteByTokenAsync(_secondToken));

        private async Task Execute(Func<Task> action)
        {
            await action();
        }
    }
}

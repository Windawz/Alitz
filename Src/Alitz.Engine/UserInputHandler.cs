using System;
using System.Text;

using Alitz.Components;
using Alitz.Systems;

namespace Alitz;
public class UserInputHandler
{
    public UserInputHandler(EntityComponentSystem ecs)
    {
        _ecs = ecs;
        _userInputComponentHolderEntity = ecs.EntityPool.Fetch();
    }

    private readonly StringBuilder _buffer = new(128);

    private readonly EntityComponentSystem _ecs;
    private readonly Id _userInputComponentHolderEntity;

    public void Handle(ConsoleKeyInfo? maybeKeyInfo)
    {
        switch (maybeKeyInfo)
        {
            case null:
                _ecs.Do(
                    _userInputComponentHolderEntity,
                    (ref CurrentUserInputLineComponent userInput) =>
                    {
                        userInput = CurrentUserInputLineComponent.None;
                    });
                break;
            case { Key: ConsoleKey.Enter, }:
                _ecs.Do(
                    _userInputComponentHolderEntity,
                    (ref CurrentUserInputLineComponent userInput) =>
                    {
                        userInput = _buffer.ToString();
                    });
                break;
            default:
                char character = maybeKeyInfo.Value.KeyChar;
                if (!char.IsControl(character))
                {
                    _buffer.Append(character);
                }
                break;
        }
    }
}

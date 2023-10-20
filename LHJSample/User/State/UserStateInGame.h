#pragma once

#include "UserState.h"

class User;

class UserStateInGame : public UserStateBase
{
public:
    virtual void Enter();
    virtual void Execute();
    virtual void Exit();

public:
    UserStateInGame(User* user);
    virtual ~UserStateInGame();
};
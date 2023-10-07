#pragma once

enum class RESULT_CODE : unsigned short
{
	NONE = 0,

	USER_MGR_INVALID_USER_INDEX = 10,
	USER_MGR_INVALID_USER_UNIQUEID = 11,

	CREATE_ACCOUNT_FAIL = 20,

	LOGIN_SUCCESS = 30,
	LOGIN_USER_ALREADY = 31,
	LOGIN_USER_USED_ALL_OBJ = 32,
	LOGIN_USER_INVALID_PW = 33,

	NEW_ROOM_USED_ALL_OBJ = 40,
	NEW_ROOM_FAIL_ENTER = 41,

	ENTER_ROOM_SUCCESS = 50,
	ENTER_ROOM_NOT_FINDE_USER = 51,
	ENTER_ROOM_INVALID_USER_STATUS = 52,
	ENTER_ROOM_NOT_USED_STATUS = 53,
	ENTER_ROOM_FULL_USER = 54,

	ROOM_INVALID_INDEX = 60,
	ROOM_NOT_USED = 61,
	ROOM_TOO_MANY_PACKET = 62,
	ROOM_FULL = 63,

	LEAVE_ROOM_SUCCESS = 70,
	LEAVE_ROOM_FAIL = 71,

	CHAT_ROOM_INVALID_ROOM_NUMBER = 80,

	LOBBY_INFO_SUCCESS = 90,

	CHAT_ROOM_SUCCESS = 100,
};
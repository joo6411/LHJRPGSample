#pragma once
#include <string>
#include <map>
#include <unordered_map>
#include <functional>
#include "../Packet.h"
#include "State/UserState.h"

class ClientInfo;
class UserManager;
enum class RESULT_CODE : unsigned short;

class User
{
	const UINT32 PACKET_DATA_BUFFER_SIZE = 8096;

public:
	User() = default;
	~User() = default;

	void Init(const INT32 index, UserManager* userManager);
	void Clear();
	int SetLogin(std::string& userID_);
	INT32 GetCurrentRoomIndex() { return mRoomIndex; }
	INT32 GetNetConnIdx() { return mIndex; }

	std::string GetUserId() const { return  mUserID; }
	void SetPacketData(const UINT32 dataSize_, char* pData_);
	PacketInfo GetPacket();
	void ChangeState(UserState newState);
	UserState GetState() { return mState; }
	void ResetRecvFunction();
	bool CreateAccount(std::string& userID, std::string& password);
	RESULT_CODE CheckLoginable(UINT32 clientIndex_, std::string&  userID, std::string& password);

	std::unordered_map<int, std::function<void(UINT16, char*)>> recvFuntionDictionary;
	std::function<void(UINT32, UINT32, char*)> sendPacketFunc;

private:
	INT32 mIndex = -1;
	INT32 mRoomIndex = -1;

	std::string mUserID;
	bool mIsConfirm = false;
	std::string mAuthToken;

	UserState mState;
	std::map<UserState, UserStateBase*> mStates;

	UINT32 mPacketDataBufferWPos = 0;
	UINT32 mPacketDataBufferRPos = 0;
	char* mPacketDataBuffer = nullptr;

	UserManager* mUserManager;
};
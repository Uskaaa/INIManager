namespace INIManager.Components.Services.Interfaces;

public interface ILockService
{
    bool SetLock(int id);
    bool IsLocked(int id);
    bool RemoveLock(int id);
    int? GetCurrentLockId();
}
#pragma once

#include "pad_trigger_effect.h"
#include "../shared/ipc_protocol.h"

namespace psvr2_toolkit {

  class TriggerEffectManager {
  public:
    TriggerEffectManager();

    static TriggerEffectManager *Instance();

    bool Initialized();
    void Initialize();

    void HandleIpcCommand(uint32_t processId, ipc::CommandHeader_t *pHeader, void *pData);

    // Battery level query (0-100, or -1 if unavailable)
    int GetControllerBatteryLevel(ipc::EVRControllerType controllerType);

  private:
    static psvr2_toolkit::TriggerEffectManager *m_pInstance;

    bool m_initialized;

    void SetTriggerEffectCommand(uint32_t processId, ipc::EVRControllerType controllerType, ScePadTriggerEffectCommand command);
  };

} // psvr2_toolkit

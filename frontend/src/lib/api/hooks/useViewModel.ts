import { PrimitiveAtom, useSetAtom } from 'jotai'
import type { Unsubscribe } from 'nanoevents'
import { useEffect, useMemo, useRef, useState } from 'react'
import {
  createDispatcher,
  createInitViewHandler,
  createInvoker,
  createSubscriber
} from '../handlers/ipcHandler'
import type { CommandEnvelope, EventEnvelope, ViewId } from '../types/apiTypes'

export type ViewModel<TEvent extends EventEnvelope, TCommand extends CommandEnvelope> = ReturnType<
  typeof useViewModel<TEvent, TCommand>
>

const useViewModel = <TEvent extends EventEnvelope, TCommand extends CommandEnvelope>(
  viewType: string
) => {
  const viewId = useMemo(() => crypto.randomUUID() as ViewId, [])

  const dispatch = useMemo(() => createDispatcher<TCommand>(viewId), [viewId])
  const subscribe = useMemo(() => createSubscriber<TEvent>(viewId), [viewId])
  const invoke = useMemo(() => createInvoker<TEvent, TCommand>(viewId), [viewId])
  const useViewState = useMemo(() => createViewStateHook<TEvent>(subscribe), [subscribe])
  const { init, dispose } = useMemo(
    () => createInitViewHandler(viewId, viewType),
    [viewId, viewType]
  )

  const [isInitialized, setIsInitialized] = useState(false)

  // 初期化と破棄のコマンド発行
  useEffect(() => {
    init().then(() => setIsInitialized(true))
    return dispose
  }, [init, dispose])

  return {
    dispatch,
    subscribe,
    invoke,
    useViewState,
    viewId,
    isInitialized
  }
}

const createViewStateHook = <TEvent extends EventEnvelope>(
  subscribeEvent: <TName extends TEvent['event']>(
    eventName: TName,
    callback: (payload: Extract<TEvent, { event: TName }>['payload']) => void
  ) => Unsubscribe
) => {
  return function useViewState<TName extends TEvent['event']>(eventName: TName) {
    const [state, setState] = useState<Extract<TEvent, { event: TName }>['payload']>()
    useEffect(() => subscribeEvent(eventName, setState), [eventName])
    return state
  }
}

export default useViewModel

export const useProvideViewModel = <TViewModel>(
  useViewModelInternal: () => TViewModel,
  atom: PrimitiveAtom<TViewModel>
) => {
  const viewModel = useViewModelInternal()
  const atomRef = useRef(atom)
  const setViewModelValue = useSetAtom(atomRef.current)

  useEffect(() => {
    setViewModelValue(viewModel)
  }, [viewModel, setViewModelValue])
}

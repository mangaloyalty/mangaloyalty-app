interface Window {
  oni?: {
    addEventListener: <TValue, TResult>(eventName: string, handler: (value?: TValue) => Promise<TResult> | TResult) => void;
    dispatchAsync: <TValue, TResult>(eventName: string, value?: TValue) => Promise<TResult>;
    removeEventListener: <TValue, TResult>(eventName: string, handler: (value?: TValue) => Promise<TResult> | TResult) => void;
    sendAsync: <TValue, TResult>(eventName: string, value?: TValue) => Promise<TResult>;
  };
}

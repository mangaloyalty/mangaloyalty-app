interface Window {
  oni?: {
    addEventListener: <T>(eventName: string, handler: (value?: T) => void) => void;
    dispatchEvent: <T>(eventName: string, value?: T) => void;
    removeEventListener: <T>(eventName: string, handler: (value?: T) => void) => void;
    sendAsync: <TValue, TResult>(eventName: string, value?: TValue) => Promise<TResult>;
  };
}

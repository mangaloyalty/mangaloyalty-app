export function proxyAsync(eventSource: string, functionName: string, parameters: {[key: string]: boolean | number | string | undefined}) {
  return window.oni?.sendAsync(`proxy.forwardAsync`, {eventSource, functionName, parameters}) as Promise<any>;
}

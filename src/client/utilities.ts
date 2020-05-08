export function proxyAsync(ev: {[key: string]: boolean | number | string | undefined}) {
  return window.oni?.sendAsync(`proxy.forwardAsync`, ev) as Promise<any>;
}

export function toBlob(value: string) {
  const a = [];
  const b = atob(value);
  for (let i = 0; i < b.length; i += 512) {
    const s = b.slice(i, i + 512);
    const v = new Array(s.length);
    for (let j = 0; j < s.length; j++) v[j] = s.charCodeAt(j);
    a.push(new Uint8Array(v));
  }
  return new Blob(a);
}

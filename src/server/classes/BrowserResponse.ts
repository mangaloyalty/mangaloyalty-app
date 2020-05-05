import * as sv from 'mangaloyalty-server';

export class BrowserResponse implements sv.IBrowserResponse {
  private readonly _id: string;
  private readonly _url: string;

  constructor(id: string, url: string) {
    this._id = id;
    this._url = url;
  }

  async bufferAsync() {
    const id = this._id;
    const url = this._url;
    const result = await window.oni?.sendAsync('browser.responseAsync', {id, url});
    return new Buffer(String(result), 'base64');
  }

  get url() {
    return this._url;
  }
}

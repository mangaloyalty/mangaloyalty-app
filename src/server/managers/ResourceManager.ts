import * as sv from 'mangaloyalty-server';

export class ResourceManager implements sv.IResourceManager {
  async moveAsync(absoluteFromPath: string, absoluteToPath: string) {
    const success = await window.oni?.sendAsync('resource.moveAsync', {absoluteFromPath, absoluteToPath});
    if (!success) throw missingError();
  }
  
  async readdirAsync(absolutePath: string) {
    return await window.oni?.sendAsync('resource.readdirAsync', {absolutePath}) as string[];
  }

  async readFileAsync(absolutePath: string) {
    const result = await window.oni?.sendAsync('resource.readFileAsync', {absolutePath});
    if (!result) throw missingError();
    return new Buffer(String(result), 'base64');
  }

  async readJsonAsync<T>(absolutePath: string) {
    const result = await window.oni?.sendAsync('resource.readJsonAsync', {absolutePath});
    if (!result) throw missingError();
    return result as T;
  }

  async removeAsync(absolutePath: string) {
    await window.oni?.sendAsync('resource.removeAsync', {absolutePath});
  }

  async writeFileAsync<T>(absolutePath: string, value: T) {
    if (Buffer.isBuffer(value)) {
      const buffer = value.toString('base64');
      await window.oni?.sendAsync('resource.writeFileAsync', {absolutePath, buffer});
    } else {
      await window.oni?.sendAsync('resource.writeJsonAsync', {absolutePath, value});
    }
  }
}

function missingError() {
  const error = new Error();
  (error as any).code = 'ENOENT';
  return error;
}

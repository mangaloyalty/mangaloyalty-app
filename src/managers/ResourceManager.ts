import * as sv from 'mangaloyalty-server';

export class ResourceManager implements sv.IResourceManager {
  moveAsync(_absoluteFromPath: string, _absoluteToPath: string): Promise<void> {
    throw new Error('TODO');
  }
  
  async readdirAsync(_absolutePath: string): Promise<string[]> {
    throw new Error('TODO');
  }

  readFileAsync(_absolutePath: string): Promise<Buffer> {
    throw new Error('TODO');
  }

  async readJsonAsync<T>(_absolutePath: string): Promise<T> {
    throw new Error('TODO');
  }

  removeAsync(_absolutePath: string): Promise<void> {
    throw new Error('TODO');
  }

  async writeFileAsync<T>(_absolutePath: string, _value: T): Promise<void> {
    throw new Error('TODO');
  }
}

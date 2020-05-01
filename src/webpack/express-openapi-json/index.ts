export function createCore() {
  const mock = () => core;
  const core = {controller: mock, router: mock};
  return core;
}

export function createOperation() {
  return;
}

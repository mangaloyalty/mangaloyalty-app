export function native() {
  if (window.oni) return window.oni;
  throw new Error('Please run as Xamarin-app!');
}

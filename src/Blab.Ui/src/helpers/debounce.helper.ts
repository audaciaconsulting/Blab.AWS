// A function to add debounce to a function
export function debounce(cb: any, delay: number = 1000) {
  // Create a new timeout function
  let timeout: any = null;
  // Pass in the arguments for your callback function here
  return (...args: any[]) => {
    // ...args are parameters ('...' lets you pass in a infinite amount of data as an array (similar to 'params' in C#))
    // ...args is the arguments for the callback function
    // Clears the timer if the call back function was called before the delay
    clearTimeout(timeout);
    // Set the new timeout
    timeout = setTimeout(() => {
      // If no changes were made before the delay it will execute the callback function and pass in the '..args' parameters
      cb(...args);
    }, delay);
  };
}

import { configureStore } from '@reduxjs/toolkit';
import LoginReducer from './features/loginSlice';
export const store = configureStore({
  reducer: {
    login: LoginReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

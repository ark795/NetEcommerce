import { configureStore } from '@reduxjs/toolkit'
import authReducer from '@/app/features/auth/authSlice'
import cartReducer from '@/app/features/cart/cartSlice'

export const store = configureStore({
  reducer: {
    auth: authReducer,
    cart: cartReducer,
  },
})

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch
import { useAppDispatch, useAppSelector } from '@/app/hooks/useRedux'
import { increaseQuantity, decreaseQuantity, removeFromCart } from '@/app/features/cart/cartSlice'
import Link from 'next/link'
import { Button } from '@/app/components/ui/button'

export default function CartPage() {
  const cart = useAppSelector((state) => state.cart.items)
  const dispatch = useAppDispatch()

  const total = cart.reduce((acc, item) => acc + item.price * item.quantity, 0)

  return (
    <div className="max-w-4xl mx-auto py-10 px-4">
      <h2 className="text-2xl font-bold mb-6">🛒 سبد خرید</h2>
      {cart.length === 0 ? (
        <p>سبد خرید شما خالی است</p>
      ) : (
        <>
          <div className="space-y-4">
            {cart.map(item => (
              <div key={item.id} className="border rounded p-4 flex justify-between items-center">
                <div>
                  <h3 className="font-semibold">{item.name}</h3>
                  <p className="text-sm text-gray-600">{item.brand}</p>
                  <p>قیمت: {item.price.toLocaleString()} تومان</p>
                  <p>تعداد: {item.quantity}</p>
                  <div className="space-x-2 mt-2">
                    <Button size="sm" onClick={() => dispatch(increaseQuantity(item.id))}>+</Button>
                    <Button size="sm" onClick={() => dispatch(decreaseQuantity(item.id))}>-</Button>
                    <Button size="sm" variant="destructive" onClick={() => dispatch(removeFromCart(item.id))}>
                      حذف
                    </Button>
                  </div>
                </div>
                <img src={item.imageUrl} className="w-24 h-24 object-cover rounded" />
              </div>
            ))}
          </div>
          <div className="mt-6 text-lg font-bold">
            جمع کل: {total.toLocaleString()} تومان
          </div>
          <Link href="/checkout">
            <Button className="mt-4 w-full">ادامه به پرداخت</Button>
          </Link>
        </>
      )}
    </div>
  )
}
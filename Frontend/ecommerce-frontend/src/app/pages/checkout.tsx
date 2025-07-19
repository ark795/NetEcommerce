import { useAppSelector, useAppDispatch } from '@/hooks/useRedux'
import { clearCart } from '@/app/features/cart/cartSlice'
import { Button } from '@/app/components/ui/button'
import { useRouter } from 'next/router'
import axios from '@/app/lib/axios'

export default function CheckoutPage() {
  const cart = useAppSelector((state) => state.cart.items)
  const dispatch = useAppDispatch()
  const router = useRouter()

  const total = cart.reduce((acc, item) => acc + item.price * item.quantity, 0)

  const handleSubmit = async () => {
    try {
      await axios.post('/order/create', {
        items: cart.map(item => ({
          productId: item.id,
          quantity: item.quantity
        })),
        totalPrice: total,
      })

      dispatch(clearCart())
      router.push('/thank-you')
    } catch (err) {
      alert('خطا در ثبت سفارش')
    }
  }

  return (
    <div className="max-w-2xl mx-auto py-10 px-4">
      <h2 className="text-2xl font-bold mb-6">💳 پرداخت</h2>
      <p>مجموع کل: <strong>{total.toLocaleString()} تومان</strong></p>
      <Button className="mt-6 w-full" onClick={handleSubmit}>پرداخت و ثبت سفارش</Button>
    </div>
  )
}

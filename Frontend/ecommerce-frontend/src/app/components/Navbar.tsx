import { useAppSelector } from '@/app/hooks/useRedux'
import Link from 'next/link'

export const Navbar = () => {
  const cartCount = useAppSelector((state) =>
    state.cart.items.reduce((acc, item) => acc + item.quantity, 0)
  )

  return (
    <header className="flex items-center justify-between px-6 py-4 shadow">
      <Link href="/" className="font-bold text-xl">E-Commerce</Link>
      <nav className="space-x-4">
        <Link href="/cart">
          🛒 سبد خرید ({cartCount})
        </Link>
        <Link href="/login">ورود</Link>
      </nav>
    </header>
  )
}
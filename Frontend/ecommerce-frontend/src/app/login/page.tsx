'use client'

import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { loginSchema, LoginSchemaType } from '@/app/lib/validations/auth'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { useAppDispatch, useAppSelector } from '@/app/hooks/useRedux'
import { login } from '@/app/features/auth/authSlice'
import { useEffect } from 'react'
import { useRouter } from 'next/router'

export default function LoginPage() {
  const dispatch = useAppDispatch()
  const { user, loading, error } = useAppSelector((state) => state.auth)
  const router = useRouter()

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginSchemaType>({
    resolver: zodResolver(loginSchema),
  })

  const onSubmit = (data: LoginSchemaType) => {
    dispatch(login(data))
  }

  useEffect(() => {
    if (user) {
      router.push('/')
    }
  }, [user])

  return (
    <div className="max-w-md mx-auto py-20 px-4">
      <h2 className="text-2xl font-bold mb-6">ورود به حساب کاربری</h2>
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <div>
          <Input placeholder="ایمیل" {...register('email')} />
          {errors.email && (
            <p className="text-red-500 text-sm">{errors.email.message}</p>
          )}
        </div>
        <div>
          <Input
            placeholder="رمز عبور"
            type="password"
            {...register('password')}
          />
          {errors.password && (
            <p className="text-red-500 text-sm">{errors.password.message}</p>
          )}
        </div>
        {error && <p className="text-red-600">{error}</p>}
        <Button type="submit" className="w-full" disabled={loading}>
          {loading ? 'در حال ورود...' : 'ورود'}
        </Button>
      </form>
    </div>
  )
}
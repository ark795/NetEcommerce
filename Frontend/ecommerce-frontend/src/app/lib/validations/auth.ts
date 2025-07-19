import { z } from "zod";

export const loginSchema = z.object({
    email: z.string().email({ message: "Email Not Valid"}),
    password: z.string().min(6, {message: "Password Should Be At Least 6 Characters"})
})

export type LoginSchemaType = z.infer<typeof loginSchema>
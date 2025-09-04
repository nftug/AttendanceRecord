import { zodResolver } from '@hookform/resolvers/zod'
import { useMutation } from '@tanstack/react-query'
import { useEffect, useRef } from 'react'
import { FieldValues, useForm } from 'react-hook-form'
import { ZodType } from 'zod'

export type UseResourceEditFormOptions<TResponse, TFormFields extends FieldValues> = {
  resourceData: TResponse | undefined
  schema: ZodType<TFormFields, TFormFields>
  toFormFields: (data: TResponse | undefined) => TFormFields
  saveFn: (formData: TFormFields) => Promise<unknown>
  onSuccess?: () => void
  onError?: (error: unknown) => void
}

export const useResourceEditForm = <TResponse, TFormFields extends FieldValues>({
  resourceData,
  schema,
  toFormFields,
  saveFn,
  onSuccess,
  onError
}: UseResourceEditFormOptions<TResponse, TFormFields>) => {
  const form = useForm({ resolver: zodResolver(schema), mode: 'onChange' })
  const toFormFieldsRef = useRef(toFormFields)
  toFormFieldsRef.current = toFormFields

  useEffect(() => {
    form.reset(toFormFieldsRef.current(resourceData))
  }, [resourceData, form])

  const mutation = useMutation({
    mutationFn: (formData: TFormFields) => saveFn(formData),
    onSuccess,
    onError
  })

  return { form, mutation }
}

export default useResourceEditForm

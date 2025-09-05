import { standardSchemaResolver } from '@hookform/resolvers/standard-schema'
import { useMutation } from '@tanstack/react-query'
import { useEffect, useRef } from 'react'
import { FieldValues, useForm } from 'react-hook-form'
import { ZodType } from 'zod'

export type UseResourceEditFormOptions<TResponse, TFormFields extends FieldValues> = {
  resourceData: TResponse | undefined
  schema: ZodType<TFormFields>
  toFormFields: (data: NonNullable<TResponse>) => TFormFields
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
  const form = useForm<TFormFields>({ resolver: standardSchemaResolver(schema), mode: 'onChange' })
  const toFormFieldsRef = useRef(toFormFields)
  toFormFieldsRef.current = toFormFields

  useEffect(() => {
    form.reset(resourceData ? toFormFieldsRef.current(resourceData) : schema.parse({}))
  }, [resourceData, form, schema])

  const mutation = useMutation({
    mutationFn: (formData: TFormFields) => saveFn(formData),
    onSuccess,
    onError
  })

  return { form, mutation }
}

export default useResourceEditForm

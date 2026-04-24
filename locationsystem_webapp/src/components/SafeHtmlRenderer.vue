<!-- components/SafeHtmlRenderer.vue -->
<template>
  <span v-html="sanitizedContent"></span>
</template>

<script>
import DOMPurify from 'dompurify'

export default {
  name: 'SafeHtmlRenderer',
  props: {
    html: {
      type: String,
      default: ''
    }
  },
  computed: {
    sanitizedContent() {
      return DOMPurify.sanitize(this.html || '')
    }
  }
}
</script>

/* Small, dependency-free highlighter for the C# and XAML samples. */
'use strict';

const csharpKeywords = new Set(('abstract as async await base bool break byte case catch char class const ' +
  'continue decimal default delegate do double else enum event false finally float for foreach if in int ' +
  'interface internal is lock long namespace new null object out override params private protected public ' +
  'readonly record ref return sealed short static string struct switch this throw true try typeof uint ulong ' +
  'using var virtual void while yield').split(/\s+/));

function tokenizeCode(source, language) {
  const pattern = language === 'xaml'
    ? /<!--[\s\S]*?(?:-->|$)|<\?[\w:.-]+|<\/?[\w:.-]+|\b[\w:.-]+(?=\s*=)|"[^"]*"|'[^']*'|\/?\s*>|&(?:#\d+|#x[\da-fA-F]+|[\w]+);/g
    : /\/\*[\s\S]*?(?:\*\/|$)|\/\/[^\r\n]*|\$?"""[\s\S]*?"""|\$?@"(?:""|[^"])*"|@\$"(?:""|[^"])*"|\$?"(?:\\.|[^"\\])*"|'(?:\\.|[^'\\])*'|\b\d+(?:\.\d+)?(?:[eE][+-]?\d+)?[fFdDmMuUlL]*\b|\b[A-Za-z_]\w*\b/g;
  const tokens = [];
  let index = 0;
  for (const match of source.matchAll(pattern)) {
    if (match.index > index) tokens.push({ text: source.slice(index, match.index), kind: '' });
    const text = match[0];
    let kind = '';
    if (language === 'xaml') {
      if (text.startsWith('<!--')) kind = 'comment';
      else if (text.startsWith('<') || /^\/?\s*>$/.test(text)) kind = 'tag';
      else if (/^["']/.test(text)) kind = 'string';
      else if (text.startsWith('&')) kind = 'number';
      else kind = 'attribute';
    } else {
      if (text.startsWith('//') || text.startsWith('/*')) kind = 'comment';
      else if (/^(?:\$?@?"|@\$"|')/.test(text)) kind = 'string';
      else if (/^\d/.test(text)) kind = 'number';
      else if (csharpKeywords.has(text)) kind = 'keyword';
      else if (/^[A-Z]/.test(text)) kind = 'type';
    }
    tokens.push({ text, kind });
    index = match.index + text.length;
  }
  if (index < source.length) tokens.push({ text: source.slice(index), kind: '' });
  return tokens;
}

function highlightCode(element, source, language) {
  const fragment = document.createDocumentFragment();
  for (const token of tokenizeCode(source, language)) {
    if (!token.kind) fragment.appendChild(document.createTextNode(token.text));
    else {
      const span = document.createElement('span');
      span.className = `syntax-${token.kind}`;
      span.textContent = token.text;
      fragment.appendChild(span);
    }
  }
  element.replaceChildren(fragment);
}

if (typeof module !== 'undefined') module.exports = { tokenizeCode };

using System;
using System.Threading;
using TMPro;
using UnityEngine;

namespace JimmysUnityUtilities.InputFieldValidators
{
    [CreateAssetMenu(menuName = "JUU/Input Field Validators/Advanced Number Validator", fileName = "Advanced Number Validator")]
    public class AdvancedNumberValidator : TMP_InputValidator
    {
        [SerializeField] bool AllowDecimals = true;
        [SerializeField] bool AllowNumbersBelowZero = true;

        public override char Validate(ref string text, ref int caretPosition, char typedCharacter)
        {
            bool cursorBeforeDash = (caretPosition == 0 && text.Length > 0 && text[0] == '-');
            if (cursorBeforeDash)
                goto disallow; // Disallow typing anything before a negative sign


            if (typedCharacter >= '0' && typedCharacter <= '9')
                goto allow; // Allow typing digits


            if (AllowNumbersBelowZero)
            {
                if (typedCharacter == '-' && caretPosition == 0)
                    goto allow; // Allow typing a negative sign at the start of the string
            }
            if (AllowDecimals)
            {
                string decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                if (typedCharacter == Convert.ToChar(decimalSeparator) && !text.Contains(decimalSeparator))
                    goto allow; // Allow typing a single decimal character in the string
            }

            goto disallow; // Disallow anything else


            allow:
            text = text.Insert(caretPosition, typedCharacter.ToString());
            caretPosition += 1;
            return typedCharacter;

            disallow:
            return (char)0;
        }
    }
}

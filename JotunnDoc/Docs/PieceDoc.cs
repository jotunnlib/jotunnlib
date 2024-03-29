﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using JotunnLib.Managers;
using JotunnLib.Utils;

namespace JotunnDoc.Docs
{
    public class PieceDoc : Doc
    {
        public PieceDoc() : base("JotunnDoc/Docs/conceptual/pieces/piece-list.md")
        {
            PieceManager.Instance.PieceRegister += docPieces;
        }

        public void docPieces(object sender, EventArgs e)
        {
            Debug.Log("Documenting pieces");

            AddHeader(1, "Piece list");
            AddText("All of the pieces currently in the game.");
            AddText("This file is automatically generated from Valheim using the JotunnDoc mod found on our GitHub.");
            AddTableHeader("Piece Table", "Prefab Name", "Piece Name", "Piece Description");

            var pieceTables = ReflectionUtils.GetPrivateField<Dictionary<string, PieceTable>>(PieceManager.Instance, "pieceTables");

            foreach (var pair in pieceTables)
            {
                foreach (GameObject obj in pair.Value.m_pieces)
                {
                    Piece piece = obj.GetComponent<Piece>();
                    AddTableRow(
                        pair.Key,
                        obj.name,
                        JotunnDoc.Localize(piece.m_name),
                        JotunnDoc.Localize(piece.m_description));
                }
            }

            Save();
        }
    }
}

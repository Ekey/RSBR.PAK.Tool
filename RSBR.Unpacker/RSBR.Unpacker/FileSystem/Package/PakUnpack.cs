using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace RSBR.Unpacker
{
    class PakUnpack
    {
        private static List<PakEntry> m_EntryTable = new List<PakEntry>();

        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            PakHashList.iLoadProject();
            using (FileStream TPakStream = File.OpenRead(m_Archive))
            {
                var m_Header = new PakHeader();

                m_Header.dwMagic = TPakStream.ReadUInt32();
                m_Header.dwTotalFiles = TPakStream.ReadInt32();
                m_Header.dwEntryTableOffset = TPakStream.ReadUInt32();

                if (m_Header.dwMagic != 0x50414B31)
                {
                    Utils.iSetError("[ERROR]: Invalid magic of PAK (OBB) archive file!");
                    return;
                }

                //Skip 1024 bytes (idk what this)
                TPakStream.Seek(1024, SeekOrigin.Current);

                m_EntryTable.Clear();
                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    var m_Entry = new PakEntry();

                    m_Entry.dwHashName = TPakStream.ReadUInt32();
                    m_Entry.dwUnknown = TPakStream.ReadUInt32();
                    m_Entry.dwDecompressedSize = TPakStream.ReadInt32();
                    m_Entry.dwCompressedSize = TPakStream.ReadInt32();
                    m_Entry.dwOffset = TPakStream.ReadUInt32();

                    m_EntryTable.Add(m_Entry);
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FileName = PakHashList.iGetNameFromHashList(m_Entry.dwHashName);
                    String m_FullPath = m_DstFolder + m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    TPakStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);
                    var lpBuffer = TPakStream.ReadBytes(m_Entry.dwCompressedSize);
                    
                    lpBuffer = PakCipher.iDecryptData(lpBuffer, m_Entry.dwOffset);

                    if (m_Entry.dwCompressedSize != m_Entry.dwDecompressedSize)
                    {
                        var lpTemp = Zlib.iDecompress(lpBuffer);
                        File.WriteAllBytes(m_FullPath, lpTemp);
                    }
                    else
                    {
                        File.WriteAllBytes(m_FullPath, lpBuffer);
                    }
                }

                TPakStream.Dispose();
            }
        }
    }
}
